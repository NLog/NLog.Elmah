﻿// Copyright 2013 Kim Christensen, Todd Meinershagen, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.

using System;

namespace NLog.Elmah.Example
{
	class Program
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		static void Main(string[] args)
		{
			Logger.Error(new ArgumentException(), "This is a message from the Program type.");
			var service = new Service();
			service.Execute();
		}
	}

	public class Service
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public void Execute()
		{
			Logger.Info("This is a message from the Service type.");
		}
	}
}
