﻿/* Copyright 2010-2012 10gen Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace MongoDB.DriverUnitTests.Jira
{
    [TestFixture]
    public class CSharp330Tests
    {
        private MongoServer _server;
        private MongoDatabase _database;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            _server = Configuration.TestServer;
            _database = Configuration.TestDatabase;
        }

        [Test]
        public void TestMongoGridFSSettingsInheritsSafeModeFromDatabase()
        {
            var gridFS = _database.GridFS;
            Assert.AreEqual(true, gridFS.Settings.WriteConcern.Enabled);
        }
    }
}
