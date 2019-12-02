using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool used = false;

    public bool Use()
    {
        bool available = !used;
        used = true;

        return available;
    }
}
