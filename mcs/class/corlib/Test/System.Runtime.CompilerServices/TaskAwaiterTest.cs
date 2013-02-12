//
// TaskAwaiterTest.cs
//
// Authors:
//	Marek Safar  <marek.safar@gmail.com>
//
// Copyright (C) 2011 Xamarin, Inc (http://www.xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#if NET_4_5

using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Runtime.CompilerServices;

namespace MonoTests.System.Runtime.CompilerServices
{
	[TestFixture]
	public class TaskAwaiterTest
	{
		[Test]
		public void GetResultFaulted ()
		{
			TaskAwaiter awaiter;

			var task = new Task (() => { throw new ApplicationException (); });
			awaiter = task.GetAwaiter ();
			task.RunSynchronously (TaskScheduler.Current);


			Assert.IsTrue (awaiter.IsCompleted);

			try {
				awaiter.GetResult ();
				Assert.Fail ();
			} catch (ApplicationException) {
			}
		}

		[Test]
		public void GetResultNotCompleted ()
		{
			TaskAwaiter awaiter;

			var task = new Task (() => { });
			awaiter = task.GetAwaiter ();

			try {
				awaiter.GetResult ();
				Assert.Fail ();
			} catch (InvalidOperationException) {
			}
		}

		[Test]
		public void GetResultCanceled ()
		{
			TaskAwaiter awaiter;

			var token = new CancellationToken (true);
			var task = new Task (() => { }, token);
			awaiter = task.GetAwaiter ();

			try {
				awaiter.GetResult ();
				Assert.Fail ();
			} catch (TaskCanceledException) {
			}
		}
	}
}

#endif