/*

The contents of this file are subject to the Mozilla Public License
Version 1.1 (the "License"); you may not use this file except in
compliance with the License. You may obtain a copy of the License at
http://www.mozilla.org/MPL/

Software distributed under the License is distributed on an "AS IS"
basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
License for the specific language governing rights and limitations
under the License.

The Original Code is OpenFAST.

The Initial Developer of the Original Code is The LaSalle Technology
Group, LLC.  Portions created by Shariq Muhammad
are Copyright (C) Shariq Muhammad. All Rights Reserved.

Contributor(s): Shariq Muhammad <shariq.muhammad@gmail.com>
                Yuri Astrakhan <FirstName><LastName>@gmail.com
*/
using System;
using System.IO;

namespace OpenFAST
{
    public sealed class PipedStream : Stream
    {
        private readonly Stream _baseStream;
        private long _position;

        public PipedStream()
        {
            _baseStream = new MemoryStream();
        }

        public PipedStream(Stream stream)
        {
            _baseStream = stream;
        }


        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get { return _baseStream.Length; }
        }

        public override long Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (_baseStream)
            {
                long oldPosition = _baseStream.Position;
                _baseStream.Position = _position;
                int bytesRead = _baseStream.Read(buffer, offset, count);
                _position = _position + bytesRead;
                _baseStream.Position = oldPosition;
                return bytesRead;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public void Write(byte[] buffer)
        {
            Write(buffer, 0, buffer.Length);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            lock (_baseStream)
            {
                long oldPosition = _baseStream.Position;
                _baseStream.Position = _position;
                _baseStream.Write(buffer, offset, count);
                _position = _position + count;
                _baseStream.Position = oldPosition;
            }
        }
    }
}