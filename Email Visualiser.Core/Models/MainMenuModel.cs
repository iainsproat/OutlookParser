﻿using System;
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
        private readonly DataAnalysisEngine _analysis;

        public MainMenuModel(DataStorage storage, DataAnalysisEngine analysisEngine)
        {
            this._data = storage;
            this._analysis = analysisEngine;
        }

        public DataStorage Data
        {
            get
            {
                return this._data;
            }
        }

        public DataAnalysisEngine AnalysisEngine
        {
            get
            {
                return this._analysis;
            }
        }

        public int NumberOfExistingEmails()
        {
            return this._data.Count;
        }

        public void DeleteAllEmails()
        {
            this._data.DeleteAll();
        }
    }
}