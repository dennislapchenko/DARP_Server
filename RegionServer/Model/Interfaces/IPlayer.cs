using MMO.Photon.Client;
using MMO.Photon.Server;

namespace RegionServer.Model.Interfaces
{
	public interface IPlayer
	{
		SubServerClientPeer Client {get; set;}
		PhotonServerPeer ServerPeer {get; set;}
		int? UserID {get; set;}
		int? CharacterID {get; set;}
	}
}

