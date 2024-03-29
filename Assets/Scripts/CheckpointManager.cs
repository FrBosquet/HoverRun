﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
  public List<GameObject> checkpoints;

  void Awake()
  {
    foreach (Transform child in transform)
    {
      checkpoints.Add(child.gameObject);
    }
  }

  public int IndexOf(GameObject checkpoint)
  {
    for (int i = 0; i < checkpoints.Count; i++)
    {
      if (checkpoints[i] == checkpoint)
      {
        return i;
      }
    }

    return -1;
  }

  public bool IsLast(GameObject checkpoint)
  {
    return IndexOf(checkpoint) == checkpoints.Count - 1;
  }

  public void Reset()
  {
    foreach (GameObject checkpoint in checkpoints)
    {
      checkpoint.SetActive(true);
    }
  }
}
