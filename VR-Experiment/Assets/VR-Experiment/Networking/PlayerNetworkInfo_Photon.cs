using Photon.Realtime;

namespace VR_Experiment.Networking
{
    public class PlayerNetworkInfo_Photon : PlayerNetworkInfo
    {
        private Player _client;

        public Player Client => _client;

        public PlayerNetworkInfo_Photon(Player client)
        {
            _client = client;
        }
    }
}
