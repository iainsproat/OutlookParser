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
        public event Action<int> EmailsCounted;

        protected void RaiseAllEmailsDeleted()
        {
            if(this.AllEmailsDeleted != null)
            {
                this.AllEmailsDeleted();
            }
        }

        protected void RaiseEmailsCounted(int count)
        {
            if(this.EmailsCounted != null)
            {
                this.EmailsCounted(count);
            }
        }

        public MainMenuModel(DataStorage storage)
        {
            this._data = storage;
        }

        public DataStorage Data //TODO we should not have to expose this
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
                int count = this._data.Count;
                this.RaiseEmailsCounted(count);
                return count;
            }
        }

        public void DeleteAllEmails()
        {
            this._data.DeleteAll();
            this.RaiseAllEmailsDeleted();
        }
    }
}
