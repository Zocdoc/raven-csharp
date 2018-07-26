#region License

// Copyright (c) 2014 The Sentry Team and individual contributors.
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted
// provided that the following conditions are met:
// 
//     1. Redistributions of source code must retain the above copyright notice, this list of
//        conditions and the following disclaimer.
// 
//     2. Redistributions in binary form must reproduce the above copyright notice, this list of
//        conditions and the following disclaimer in the documentation and/or other materials
//        provided with the distribution.
// 
//     3. Neither the name of the Sentry nor the names of its contributors may be used to
//        endorse or promote products derived from this software without specific prior written
//        permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR
// IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using NSubstitute;

using NUnit.Framework;

using SharpRaven.Data;

namespace SharpRaven.UnitTests.RavenClientTests
{
    [TestFixture]
    public class CaptureRequestTests
    {
        #region SetUp/Teardown

        [SetUp]
        public void SetUp()
        {
            this.tester = new CaptureTester();
        }

        #endregion

        [Test]
        public void BuildPacket_CapturesRequestUrlAndUser()
        {
            var request = new SentryRequest
            {
                Url = "/foo/bar"
            };

            var requestFactory = Substitute.For<ISentryRequestFactory>();
            requestFactory.Create().Returns(request);

            var user = new SentryUser("user@email.com");
            var userFactory = Substitute.For<ISentryUserFactory>();
            userFactory.Create().Returns(user);

            var client = this.tester.GetTestableRavenClient("", requestFactory, userFactory);
            var packet = client.BuildPacket(new SentryEvent(new SentryMessage("some message")));
            
            Assert.IsNotNull(packet, "the packet should not be null");
            Assert.IsNotNull(packet.Request, "the request should not be null");
            Assert.AreEqual("/foo/bar", packet.Request.Url);

            Assert.IsNotNull(packet.User, "user should not be null");
            Assert.AreEqual("user@email.com", packet.User.Username);
        }

        private CaptureTester tester;
    }
}