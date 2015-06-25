﻿/*
    The MIT License (MIT)

    Copyright (c) 2015 JC Snider, Joe Bridges
  
    Website: http://ascensiongamedev.com
    Contact Email: admin@ascensiongamedev.com

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Intersect_Client.Classes
{
    public class ByteBuffer
    {
        readonly List<byte> _buff;
        public int Readpos;
        private bool _wasUpdated;
        private byte[] _readBytes;
        public ByteBuffer()
        {
            _buff = new List<byte>();
            Readpos = 0;
        }
        public byte[] ToArray()
        {
            return _buff.ToArray();
        }
        public int Count()
        {
            return _buff.Count;
        }
        public int Pos()
        {
            return Readpos;
        }
        public int Length()
        {
            return Count() - Readpos;
        }
        public void Clear()
        {
            _buff.Clear();
            Readpos = 0;
        }
        public void WriteBytes(byte[] input)
        {
            _buff.AddRange(input);
            _wasUpdated = true;
        }
        public void WriteByte(byte input)
        {
            _buff.Add(input);
            _wasUpdated = true;
        }
        public void WriteShort(short input)
        {
            _buff.AddRange(BitConverter.GetBytes(input));
            _wasUpdated = true;
        }
        public void WriteInteger(int input)
        {
            _buff.AddRange(BitConverter.GetBytes(input));
            _wasUpdated = true;
        }
        public void WriteLong(long input)
        {
            _buff.AddRange(BitConverter.GetBytes(input));
            _wasUpdated = true;
        }
        public void WriteDouble(double input)
        {
            _buff.AddRange(BitConverter.GetBytes(input));
            _wasUpdated = true;
        }
        public void WriteString(string input)
        {
            _buff.AddRange(BitConverter.GetBytes(input.Length));
            _buff.AddRange(Encoding.ASCII.GetBytes(input));
            _wasUpdated = true;
        }
        public string ReadString()
        {
            return ReadString(true);
        }
        public string ReadString(bool peek)
        {
            var len = ReadInteger(true);
            if (_wasUpdated)
            {
                _readBytes = _buff.ToArray();
                _wasUpdated = false;
            }
            var ret = Encoding.ASCII.GetString(_readBytes, Readpos, len);
            if (peek & _buff.Count > Readpos)
            {
                if (ret.Length > 0)
                {
                    Readpos += len;
                }
            }
            return ret;
        }
        public byte ReadByte()
        {
            return ReadByte(true);
        }
        public byte ReadByte(bool peek)
        {
            //check to see if this passes the byte count
            if (_buff.Count <= Readpos) throw new Exception("Byte Buffer Past Limit!");
            var ret = _buff[Readpos];
            if (peek & _buff.Count > Readpos)
            {
                Readpos += 1;
            }
            return ret;
        }
        public byte[] ReadBytes(int length)
        {
            return ReadBytes(length, true);
        }
        public byte[] ReadBytes(int length, bool peek)
        {
            var ret = _buff.GetRange(Readpos, length).ToArray();
            if (peek)
                Readpos += length;
            return ret;
        }
        public short ReadShort()
        {
            return ReadShort(true);
        }
        public short ReadShort(bool peek)
        {
            //check to see if this passes the byte count
            if (_buff.Count <= Readpos) throw new Exception("Byte Buffer Past Limit!");
            if (_wasUpdated)
            {
                _readBytes = _buff.ToArray();
                _wasUpdated = false;
            }
            var ret = BitConverter.ToInt16(_readBytes, Readpos);
            if (peek & _buff.Count > Readpos)
            {
                Readpos += 2;
            }
            return ret;
        }
        public int ReadInteger()
        {
            return ReadInteger(true);
        }
        public int ReadInteger(bool peek)
        {
            //check to see if this passes the byte count
            if (_buff.Count <= Readpos) throw new Exception("Byte Buffer Past Limit!");
            if (_wasUpdated)
            {
                _readBytes = _buff.ToArray();
                _wasUpdated = false;
            }
            var ret = BitConverter.ToInt32(_readBytes, Readpos);
            if (peek & _buff.Count > Readpos)
            {
                Readpos += 4;
            }
            return ret;
        }
        public long ReadLong()
        {
            return ReadLong(true);
        }
        public long ReadLong(bool peek)
        {
            //check to see if this passes the byte count
            if (_buff.Count <= Readpos) throw new Exception("Byte Buffer Past Limit!");
            if (_wasUpdated)
            {
                _readBytes = _buff.ToArray();
                _wasUpdated = false;
            }
            var ret = BitConverter.ToInt64(_readBytes, Readpos);
            if (peek & _buff.Count > Readpos)
            {
                Readpos += 8;
            }
            return ret;
        }

        public double ReadDouble()
        {
            return ReadDouble(true);
        }
        public double ReadDouble(bool peek)
        {
            //check to see if this passes the byte count
            if (_buff.Count <= Readpos) throw new Exception("Byte Buffer Past Limit!");
            if (_wasUpdated)
            {
                _readBytes = _buff.ToArray();
                _wasUpdated = false;
            }
            var ret = BitConverter.ToDouble(_readBytes, Readpos);
            if (peek & _buff.Count > Readpos)
            {
                Readpos += 8;
            }
            return ret;
        }

        // To detect redundant calls
        private bool _disposedValue;

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _buff.Clear();
                }
                Readpos = 0;
            }
            _disposedValue = true;
        }

        #region " IDisposable Support "
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


    }
}
