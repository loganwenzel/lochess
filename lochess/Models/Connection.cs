﻿namespace lochess.Models
{
    public class Connection
    {
        public string ConnectionId { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
        public string AspNetUserId { get; set; }
    }
}
