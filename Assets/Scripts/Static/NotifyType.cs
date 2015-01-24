using UnityEngine;
using System.Collections;

/// Enum used for the notification system
public enum NotifyType {
	Null					= 0,

	GameInitState			= 10,
	GameDisabled			= 15,

	AnimationDone			= 20,

	LoadUpdateProgress		= 30,
	LoadFinished			= 35,

	LoadGameSceneDone       = 40,

	TouchHappened			= 50,

	AntDeath				= 100
}
