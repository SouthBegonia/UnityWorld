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

public class Gun : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private GameObject _gunModel;
    [SerializeField]
    private Game game;

    private int maxRightRotation = 130;
    private int minRightRotation = 0;
    private int maxUpRotation = -60;
    private int minUpRotation = 0;
    private float cooldown = 0.5f;
    private float cooldownTimer = 5;

    // Update is called once per frame
    void Update()
    {
        if (game.IsGamePaused())
        {
            return;
        }

        float xPosPercent = Input.mousePosition.x / Screen.width;
        float yPosPercent = Input.mousePosition.y / Screen.height;
        float xVal = Mathf.Clamp(xPosPercent * maxRightRotation, minRightRotation, maxRightRotation) - 65;
        float yVal = Mathf.Clamp(yPosPercent * maxUpRotation, maxUpRotation, minUpRotation) + 10;

        transform.eulerAngles = new Vector3(yVal, xVal, 0);
        cooldownTimer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && cooldownTimer >= cooldown)
        {
            _animator.Play("Fire");
            GameObject bullet = Instantiate(_bullet);
            bullet.GetComponent<Rigidbody>().AddForce(_gunModel.transform.forward * 700);
            cooldownTimer = 0;
            game.AddShot();
            game.bullets.Add(bullet);
        }
    }
}
