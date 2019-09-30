/*
 * Copyright (c) 2017 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int position;

    public GameObject activeRobot;

    [SerializeField]
    private GameObject[] robots;
    [SerializeField]
    private Game game;

    void Start()
    {
        GetComponent<BoxCollider>().enabled = false;

        foreach (GameObject robot in robots)
        {
            robot.SetActive(false);
        }

        if (activeRobot == null)
        {
            StartCoroutine("AliveTimer");
            StartCoroutine("DeathTimer");
        }
    }

    public void DisableRobot()
    {
        foreach (GameObject robot in robots)
        {
            robot.SetActive(false);
        }
        GetComponent<BoxCollider>().enabled = false;
        activeRobot = null;
        StopAllCoroutines();
    }

    // When hit by bullet
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.collider.gameObject);
        activeRobot.GetComponent<Animator>().Play("Die");
        game.AddHit();
        GetComponent<BoxCollider>().enabled = false;
        activeRobot = null;
        StartCoroutine("AliveTimer");
        StartCoroutine("DeathTimer");
    }

    public void ActivateRobot()
    {
        activeRobot = robots[Random.Range(0, 3)];
        activeRobot.SetActive(true);
        activeRobot.GetComponent<Animator>().Play("Rise");
        GetComponent<BoxCollider>().enabled = true;
    }

    public void ActivateRobot(RobotTypes type)
    {
        StopAllCoroutines();
        activeRobot = robots[(int)type];
        activeRobot.SetActive(true);
        activeRobot.GetComponent<Animator>().Play("Rise", 0, 1);
        GetComponent<BoxCollider>().enabled = true;
    }

    IEnumerator AliveTimer()
    {
        yield return new WaitForSeconds(Random.Range(2, 6));
        ActivateRobot();
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(Random.Range(10, 14));
        if (activeRobot == null)
        {
            yield break;
        }
        activeRobot.GetComponent<Animator>().Play("Die");
        GetComponent<BoxCollider>().enabled = false;
        activeRobot = null;
        StartCoroutine("AliveTimer");
    }

    public void RefreshTimers()
    {
        StopAllCoroutines();
        StartCoroutine("AliveTimer");
        StartCoroutine("DeathTimer");
    }

    public void ResetDeathTimer()
    {
        StartCoroutine("DeathTimer");
    }
}