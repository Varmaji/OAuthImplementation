﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuthImplementation.Models
{
    public class ProjectModel
    {
        public int count { get; set; }
        public List<Project> value { get; set; }
    }


    public class Project
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string state { get; set; }
        public int revision { get; set; }
        public string visibility { get; set; }
        public DateTime lastUpdateTime { get; set; }
    }
}