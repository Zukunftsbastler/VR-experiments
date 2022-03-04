using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] private GameObject _popUpPrefab;
    [SerializeField] private GameObject _talkingpointsPrefab;

    public void DisplayProductNotification(int actorNumber, string interaction, SO_Product product)
    {
        string title = $"Client {actorNumber} {interaction} '{product.Name}'";

        PopUpMessage popUp = Instantiate(_popUpPrefab, transform).GetComponent<PopUpMessage>();
        popUp.DisplayMessage(null, title, product.Info);
    }

    public void DisplayProductTalkingpoints(string talkingPoints)
    {
        TalkingpointsMessage message = Instantiate(_talkingpointsPrefab, transform).GetComponent<TalkingpointsMessage>();
        message.DisplayMessage(talkingPoints);
    }
}
