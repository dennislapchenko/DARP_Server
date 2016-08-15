using System;
using System.Collections.Generic;
using ComplexServerCommon;
using MMO.Framework;
using MMO.Photon.Application;
using MMO.Photon.Server;
using Photon.SocketServer;
using RegionServer.Model.Effects;
using RegionServer.Model.Items;
using RegionServer.Model.ServerEvents.CharacterEvents;

namespace RegionServer.Handlers.Character
{
    public class UseItemHandler : PhotonServerHandler
    {
        public UseItemHandler(PhotonApplication application) : base(application)
        {
        }

        public override MessageType Type { get { return MessageType.Request;} }
        public override byte Code { get { return (byte) ClientOperationCode.Region;} }
        public override int? SubCode { get { return (int) MessageSubCode.UseItem;} }
        protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
        {
            var instance = Util.GetCPlayerInstance(Server, message);
            var itemId = Convert.ToInt32(message.Parameters[(byte) ClientParameterCode.ObjectId]);

            var item = instance.Items.UseItem(itemId) as Item;

            if (item.Effect == null)
            {
                serverPeer.SendOperationResponse(new OperationResponse(message.Code, new Dictionary<byte, object> { { (byte)ClientParameterCode.PeerId, message.Parameters[(byte)ClientParameterCode.PeerId] } })
                {
                    ReturnCode = (int)ErrorCode.OperationInvalid,
                    DebugMessage = "Item cannot be used"
                }, new SendParameters());

                return true;
            }

            if (item != null)
            {
                switch (item.Effect.Type)
                {
                    case EffectType.STATONLY:
                        instance.SendPacket(new UserInfoUpdatePacket(instance));
                        break;
                    case EffectType.ONHIT:
                        break;
                    case EffectType.MISC:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return true;
        }
    }
}
