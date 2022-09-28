namespace lochess.Models
{
    public class Game
    {
        public int? Id { get; set; }
        public string BlackUserName { get; set; }
        public string WhiteUserName { get; set; }
        public bool GameActive { get; set; }
        public string? Pgn { get; set; }

        //Game.Result is either: 'draw', '{BlackUserName}', or '{WhiteUserName}' to indicate which player won
        public string? Result { get; set; }
    }
}
