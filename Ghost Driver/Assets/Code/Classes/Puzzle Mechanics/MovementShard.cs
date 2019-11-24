using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class MovementShard : MonoBehaviour
{
    private void OnTriggerStay (Collider other)
    {
        if (other.CompareTag ("Player"))
        {
            if (Input.GetKeyDown (KeyCode.W))
            {
                PuzzleSignals.MovementPressed (Vector3.up);
            }
            else if (Input.GetKeyDown (KeyCode.S))
            {
                PuzzleSignals.MovementPressed (Vector3.down);
            }
            else if (Input.GetKeyDown (KeyCode.D))
            {
                PuzzleSignals.MovementPressed (Vector3.right);
            }
            else if (Input.GetKeyDown (KeyCode.A))
            {
                PuzzleSignals.MovementPressed (Vector3.left);
            }
        }
    }
}
