namespace lochess.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public int BlackUserId { get; set; }
        public int WhiteUserId { get; set; }
        public HttpPostedFile PgnFile { get; set;  }
    }
}
