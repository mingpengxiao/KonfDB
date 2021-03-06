﻿#region License and Product Information

// 
//     This file 'ClientContextFromConfig.cs' is part of KonfDB application - 
//     a project perceived and developed by Punit Ganshani.
// 
//     KonfDB is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     KonfDB is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with KonfDB.  If not, see <http://www.gnu.org/licenses/>.
// 
//     You can also view the documentation and progress of this project 'KonfDB'
//     on the project website, <http://www.konfdb.com> or on 
//     <http://www.ganshani.com/applications/konfdb>

#endregion

using System;
using System.IO;
using KonfDB.Infrastructure.Caching;
using KonfDB.Infrastructure.Exceptions;
using KonfDB.Infrastructure.Extensions;
using KonfDB.Infrastructure.Factory;
using KonfDB.Infrastructure.Logging;
using KonfDB.Infrastructure.Shell;
using KonfDB.Infrastructure.Utilities;
using KonfDBCF.Configuration;

namespace KonfDBCF.Core
{
    /// <summary>
    ///     Application Context for Client using Config
    /// </summary>
    internal class ClientContextFromConfig
    {
        private static ClientContextFromConfig _current;
        private static ClientConfig _config;

        internal static ClientContextFromConfig Current
        {
            get
            {
                if (_config == null)
                {
                    _config = File.ReadAllText("konfdb.json").FromJsonToObject<ClientConfig>();
                }
                return _current ?? (_current = new ClientContextFromConfig(_config));
            }
        }

        private ClientContextFromConfig(ClientConfig configuration)
        {
            if (configuration == null)
                throw new InvalidOperationException(
                    "Current Context could not be initialized. Configuration may be missing in App.Config file.");

            Config = configuration;

            var logger = LogFactory.CreateInstance(configuration.Runtime.LogInfo);

            if (configuration.Runtime == null)
                throw new InvalidConfigurationException("Could not find Runtime Configuration for KonfDBCF");

            var cache = CacheFactory.Create(Config.Caching);
            cache.ItemRemoved +=
                (sender, args) =>
                    Log.Debug("Item removed from cache: " + args.CacheKey + " Reason : " + args.RemoveReason);

            CurrentContext.CreateDefault(logger, new CommandArgs(string.Empty), cache);
        }

        public BaseLogger Log
        {
            get { return CurrentContext.Default.Log; }
        }

        public BaseCacheStore Cache
        {
            get { return CurrentContext.Default.Cache; }
        }

        public IArguments ApplicationParams
        {
            get { return CurrentContext.Default.ApplicationParams; }
        }

        internal ClientConfig Config { get; private set; }
    }
}