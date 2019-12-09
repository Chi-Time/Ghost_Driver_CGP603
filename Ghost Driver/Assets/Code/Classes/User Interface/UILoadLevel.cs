using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GD.Assets.Code.Classes.User_Interface
{
    public class UILoadLevel : MonoBehaviour
    {
        /// <summary>Loads a level through the scene loader by it's given index.</summary>
        /// <param name="sceneIndex">The index of the scene to load.</param>
        public void LoadLevel (int sceneIndex)
        {
            SceneLoader.Instance.Load (sceneIndex);
        }

        /// <summary>Loads a level through the scene loader by it's given name.</summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        public void LoadLevel (string sceneName)
        {
            SceneLoader.Instance.Load (sceneName);
        }
    }
}
