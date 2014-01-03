using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OutlookParser;
using EmailVisualiser.Analysis;
using EmailVisualiser.Data;

namespace EmailVisualiser.Models
{
    public class MainMenuModel : IModel
    {
        private readonly DataStorage _data;

        public event Action AllEmailsDeleted;

        protected void RaiseAllEmailsDeleted()
        {
            if(this.AllEmailsDeleted != null)
            {
                this.AllEmailsDeleted();
            }
        }

        public MainMenuModel(DataStorage storage)
        {
            this._data = storage;
        }

        public DataStorage Data
        {
            get
            {
                return this._data;
            }
        }

        public int NumberOfExistingEmails
        {
            get
            {
                return this._data.Count;
            }
        }

        public void DeleteAllEmails()
        {
            this._data.DeleteAll();
            this.RaiseAllEmailsDeleted();
        }
    }
}
