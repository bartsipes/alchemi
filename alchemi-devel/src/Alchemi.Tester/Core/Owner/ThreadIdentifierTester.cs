#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
* Title         :  ThreadIdentifierTester.cs
* Project       :  Alchemi.Tester.Core.Owner
* Created on    :  19 April 2006
* Copyright     :  Copyright © 2006 Tibor Biro (tb@tbiro.com)
* Author        :  Tibor Biro (tb@tbiro.com)
* License       :  GPL
*                    This program is free software; you can redistribute it and/or
*                    modify it under the terms of the GNU General Public
*                    License as published by the Free Software Foundation;
*                    See the GNU General Public License
*                    (http://www.gnu.org/copyleft/gpl.html) for more 
details.
*
*/
#endregion

using System;
using System.Collections.Generic;
using System.Text;

using Alchemi.Core.Owner;

using NUnit.Framework;

namespace Alchemi.Tester.Core.Owner
{
    [TestFixture]
    public class ThreadIdentifierTester
    {
        #region "Constructor tests"
        [Test]
        public void ConstructorTestDefaultPriority()
        { 
            string applicationId = Guid.NewGuid().ToString();
            int threadId = 1234;

            ThreadIdentifier ti = new ThreadIdentifier(applicationId, threadId);

            Assert.AreEqual(applicationId, ti.ApplicationId);
            Assert.AreEqual(threadId, ti.ThreadId);
            Assert.AreEqual(ThreadIdentifier.DefaultPriority, ti.Priority);
        }

        [Test]
        public void ConstructorTestSimepleConstructor()
        {
            string applicationId = Guid.NewGuid().ToString();
            int threadId = 1234;
            int priority = 100;

            ThreadIdentifier ti = new ThreadIdentifier(applicationId, threadId, priority);

            Assert.AreEqual(applicationId, ti.ApplicationId);
            Assert.AreEqual(threadId, ti.ThreadId);
            Assert.AreEqual(priority, ti.Priority);
        }

        #endregion


    }
}
