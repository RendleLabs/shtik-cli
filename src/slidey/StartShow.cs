﻿using System;

namespace slidey
{
    public class StartShow
    {
        public string Presenter { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public DateTimeOffset Time { get; set; }
        public string Place { get; set; }
        public string Markdown { get; set; }
    }
}