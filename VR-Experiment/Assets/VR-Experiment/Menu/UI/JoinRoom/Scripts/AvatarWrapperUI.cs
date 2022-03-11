using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VR_Experiment.Avatar.Core;
using VR_Experiment.Core;
using VR_Experiment.Menu.UI.Core;

namespace VR_Experiment.Menu.UI.JoinRoom
{
    public class AvatarWrapperUI : MonoBehaviour, IItemListCallbackListener
    {
        [SerializeField] private List<SO_Avatar> _avatars;
        [SerializeField] private ScrollableItemListUI _avatarSelectionUI;

        public ScrollableItemListUI SelectionUI => _avatarSelectionUI;

        private void Awake()
        {
            _avatarSelectionUI.SetCallbackListener(this);
            _avatarSelectionUI.SetItems(_avatars.Cast<ScriptableListItem>().ToList());
        }

        public void OnItemToggleInvoked(bool isActive, string itemName)
        {
            GameObject prefab = isActive ? _avatars.FirstOrDefault(a => a.Name.Equals(itemName)).Asset : null;
            PlayerWrapper.Instance.SetAvatar(prefab, true);
        }
    }
}
