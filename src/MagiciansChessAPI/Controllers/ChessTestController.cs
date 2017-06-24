using ChessLibrary;
using MagiciansChessAPI.QuickChess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace MagiciansChessAPI.Controllers
{
    public class ChessTestController : ApiController
    {

        public string GetInitialGame()
        {
            XmlNode xml = ChessAPI.getInitialGameXML();
            Game g= new Game();
            g.Reset();
            g.ActivePlay.GetBestMove();
            g.XmlDeserialize(xml);
            Move m = g.ActivePlay.GetBestMove();
            return "hi";

           
        }
    }
}
