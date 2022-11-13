using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Console.Business
{
    internal class BigDataHandle
    {
    }


    public class TextFileLineBuffer : IEnumerable<string>
    {
        private const int QueueSize = 100000;
        private BlockingCollection<string> _buffer = new BlockingCollection<string>(QueueSize);
        private CancellationTokenSource _cancelToken;
        private StreamReader _reader;

        public TextFileLineBuffer(string filename)
        {
            // File is opened here so that any exception is thrown on the calling thread. 
            _reader = new StreamReader(filename);
            _cancelToken = new CancellationTokenSource();
            // start task that reads the file
            Task.Factory.StartNew(ProcessFile, TaskCreationOptions.LongRunning);
        }

        public string GetNextLine()
        {
            if (_buffer.IsCompleted)
            {
                // The buffer is empty because the file has been read
                // and all lines returned.
                // You can either call this an error and throw an exception,
                // or you can return null.
                return null;
            }

            // If there is a record in the buffer, it is returned immediately.
            // Otherwise, Take does a non-busy wait.

            // You might want to catch the OperationCancelledException here and return null
            // rather than letting the exception escape.

            return _buffer.Take(_cancelToken.Token);
        }

        private void ProcessFile()
        {
            while (!_reader.EndOfStream && !_cancelToken.Token.IsCancellationRequested)
            {
                var line = _reader.ReadLine();
                try
                {
                    // This will block if the buffer already contains QueueSize records.
                    // As soon as a space becomes available, this will add the record
                    // to the buffer.
                    _buffer.Add(line, _cancelToken.Token);
                }
                catch (Exception ex /*OperationCancelledException*/)
                {
                   throw ;
                }
            }
            _buffer.CompleteAdding();
        }

        public void Cancel()
        {
            _cancelToken.Cancel();
        }

        public IEnumerator<string> GetEnumerator()
        {
            foreach (var item in _buffer.GetConsumingEnumerable())
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void DoSomething(IEnumerable<string> list)
        {
            foreach (var item in list)
            {
                System.Console.WriteLine(item);
            }
        }
    }
}
