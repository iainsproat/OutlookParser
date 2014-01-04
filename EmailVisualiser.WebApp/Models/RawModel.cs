using System;
using System.Collections.Generic;

using EmailVisualiser.Data;

namespace EmailVisualiser.WebApp.Models
{
    public class RawModel
    {
        private readonly DataStorage _data;

        public RawModel(DataStorage dataStore)
        {
            this._data = dataStore;
        }

        public IEnumerable<IPersistentEmail> AllEmails
        {
            get
            {
                return this._data.AllEmails;
            }
        }
    }
}
