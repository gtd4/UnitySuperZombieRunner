﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public float delay = 2.0f;
    public bool isActive = true;
    public Vector2 delayRange = new Vector2(1, 2);

    // Use this for initialization
    private void Start()
    {
        ResetDelay();
        StartCoroutine(EnemyGenerator());
    }

    private IEnumerator EnemyGenerator()
    {
        yield return new WaitForSeconds(delay);

        if (isActive)
        {
            var newTransform = transform;
            GameObjectUtil.Instantiate(prefabs[Random.Range(0, prefabs.Length)], newTransform.position);
            ResetDelay();
        }

        StartCoroutine(EnemyGenerator());
    }

    private void ResetDelay()
    {
        delay = Random.Range(delayRange.x, delayRange.y);
    }
}