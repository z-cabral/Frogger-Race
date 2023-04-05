using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehavior : MonoBehaviour
{
    //Upon Awakening, The Missile Assigns a frog as it's target,
    //The frog Assigned should be the second closest frog. The Missile
    //will also begin a coroutine that will act as a countdown.

    //during update, the Missile should keep track of it's target frog's
    //transform, rotate itself towards the target frog, and move towards
    //the target frog.

    //upon collision with the frog, the missile will spawn a small explosion
    //and destroy itself.

    //After a set amount of time, the missile will spawn a small explosion,
    //and destroy itself.
}
