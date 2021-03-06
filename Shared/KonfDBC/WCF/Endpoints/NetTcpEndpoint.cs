﻿#region License and Product Information

// 
//     This file 'NetTcpEndpoint.cs' is part of KonfDB application - 
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

using System.ServiceModel;
using System.ServiceModel.Description;
using KonfDB.Infrastructure.Shell;
using KonfDB.Infrastructure.WCF.Interfaces;

namespace KonfDB.Infrastructure.WCF.Endpoints
{
    public class NetTcpEndpoint : IEndPoint
    {
        public ServiceEndpoint Host<T>(ServiceHost host, string serverName, string serviceName, IBinding binding)
        {
            const string addressUriFormat = "{0}://{1}:{2}/{3}/";
            string endpointAddress = string.Format(addressUriFormat, "net.tcp", serverName, binding.Configuration.Port,
                serviceName);

            return host.AddServiceEndpoint(typeof (T), binding.WcfBinding, endpointAddress);
        }

        public ServiceEndpoint HostSecured<T>(ServiceHost host, string serverName, string serviceName, IBinding binding,
            ISecurity security)
        {
            CurrentContext.Default.Log.Info("TCP does not support " + security.SecurityMode +
                                            ". Communication will be setup without SSL");

            return Host<T>(host, serverName, serviceName, binding);
        }
    }
}