﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class ControlManager : MonoBehaviour {

	public GameManager gameManager;

	public Slider throttle;
	public Button doorButton;

	public IObservable<Unit> ThrottleStreamUp {get; private set;}
	public IObservable<Unit> ThrottleStreamDown {get; private set;}
	public IObservable<Unit> DoorButtonDown {get; private set;}

	private void Awake () {

		// define what feeds the streams
		ThrottleStreamUp = this.FixedUpdateAsObservable()
			.Where( _ => Input.GetButtonDown("up") );

		ThrottleStreamDown = this.FixedUpdateAsObservable()
			.Where( _ => Input.GetButtonDown("down") );

		DoorButtonDown = doorButton.OnClickAsObservable();

		// define what happens when events are found in stream
		ThrottleStreamUp
			.Subscribe(inputMovement => { throttle.value++; })
			.AddTo(this);

		ThrottleStreamDown
			.Subscribe(inputMovement => { throttle.value--; })
			.AddTo(this);

		DoorButtonDown
			.Subscribe(input => { gameManager.doorsOpen = !gameManager.doorsOpen; })
			.AddTo(this);
	}
}