using FTD.M3.API.Models;
using FTD.M3.API.Services.Interfaces;
using M3Service;
using M3Service.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FTD.M3.API.Services
{
    public class M3RequestService : IM3Service
    {

        private IConfigurationRoot _configuration;
        private string _user;
        private string _password;
        private string _url;

        public M3RequestService()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            _configuration = builder.Build();
            _user = this._configuration.GetValue<string>("M3Settings:User");
            _password = this._configuration.GetValue<string>("M3Settings:Password");
            _url = this._configuration.GetValue<string>("M3Settings:Url");
        }


        public M3Response ExecuteM3(RequestDto request) {

            M3Client m3Client = new M3Client(this._user, this._password, this._url);

            var paramStr = "";
            if (!Object.ReferenceEquals(null, request.Param) )
            {
                paramStr = request.Param.ToString();
            }

            var result = m3Client.GetDataAsync(request.Program, request.Transaction, paramStr,
                                               request.Output != null ? request.Output.ToList() : new List<string>(), request.OutputAll);

            if (result.Success && !Object.ReferenceEquals(null, result.Data) && result.Data.Count > 0)
            {
                if (!Object.ReferenceEquals(null, request.Filter))
                {
                    var filterQuery = result.Data as IEnumerable<dynamic>;

                    var filtervalues = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.Filter.ToString());
                    foreach (var f in filtervalues)
                    {
                        filterQuery = filterQuery.Where(a => a[f.Key] == f.Value);
                    }

                    result.Data = filterQuery.ToList();
                }

                if (request.Sort != null)
                {
                    if (request.OutputAll || request.Output == null || !request.Sort.Except(request.Output).Any()) {

                        var sortQuery = result.Data as IEnumerable<dynamic>;

                        if (!request.OrderByDesc)
                        {
                            var query = sortQuery.OrderBy(a => a[request.Sort.First()]);

                            foreach (var property in request.Sort.Skip(1))
                            {
                                query = query.ThenBy(a => a[property]);
                            }
                            result.Data = query.ToList();
                        }
                        else
                        {

                            var query = sortQuery.OrderByDescending(a => a[request.Sort.First()]);

                            foreach (var property in request.Sort.Skip(1))
                            {
                                query = query.ThenByDescending(a => a[property]);
                            }
                            result.Data = query.ToList();
                        }
                    }

                }
            }

            return result;
        }
    }
}

