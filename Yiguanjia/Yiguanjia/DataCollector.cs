using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO.Ports;
using System.Threading;


namespace Yiguanjia
{
    class DataCollector
    {
        public interface FutureData
        {
            void process(byte[] buf, int N);
        }

        bool _continue;
        SerialPort _serialPort;
        Thread _reader;
        FutureData _processor;

        public DataCollector(FutureData processor)
        {
            this._serialPort = new SerialPort();
            this._serialPort.BaudRate = 115200; // baudRate;
            this._serialPort.Parity = Parity.None;
            this._serialPort.DataBits = 8; // dataBits;
            // this._serialPort.StopBits = StopBits.None;
            // this._serialPort.Handshake = null; // Handshake handshake
            this._serialPort.ReadTimeout = 100;
            this._serialPort.WriteTimeout = 1000;

            this._processor = processor;
        }

        public void Connnect(string portName)
        {
            this._serialPort.PortName = portName;
            this._continue = true;
            this._serialPort.Open();
            this._reader = new Thread(Read);
            this._reader.Start();
        }

        public void Disconnect()
        {
            this._continue = false;
            this._reader.Abort();
            this._serialPort.Close();
            this._reader = null;
        }

        public void Send(byte[] buf)
        {
            this._serialPort.Write(buf, 0, buf.Length);
        }

        public void Read()
        {
            byte[] buf = new byte[4096];
            while (this._continue)
            {
                try
                {
                    int rsize = _serialPort.Read(buf, 0, buf.Length);
                    this._processor.process(buf, rsize);
                    Console.WriteLine("{0}: {1}", rsize, buf);
                }
                catch (TimeoutException) { }
            }
        }
    }
}
