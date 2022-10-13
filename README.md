# lochess
lochess is a Chess Web App built with ASP.NET MVC, MySQL, AWS RDS, SignalR, chessboardjs, chess.js, and Bootstrap. It is a personal project that demonstrates abilities to build a full stack, real-time app with common industry standards like user authorization, relational database integration, git management, and thoughtful programming.

The app is currently at the Minimum Viable Product (MVP) stage of development, since the core functionality is implemented enough to communicate the intended use cases of the app, but there are more features to be implemented before the alpha version will be hosted. 

Current features include:
- User account management
- Session management to handle game 'sessions' and real-time server side information communication
   - Chess game invite system to allow user to start game (create session) with other users
   - Chess move communication from player-player in the given game (session)
   - Instant messaging between players in the given game (session)
- Chess game logic and UI implementation
- Chess game PGN and result recorded in database
- General MVP UI of entire app

Upcoming features include:
- Create 'friend' functionality
- Build Games and Friends pages to view all past games and list of friends
- Create functionality for replaying/analyzing past games
- Integration with the chess.com API for syncing accounts and loading in games for analysis
- Create dashboard for game analysis that shows statistics/metrics of each game
- Implement security measures for preventing XSS, SQL Injection, CSRF, etc...
- Create custom error page for error handling
- Overhaul UI with Figma, and create CSS animations for winning, losing, and drawing games
