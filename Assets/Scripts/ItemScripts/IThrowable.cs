using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
Originally used this when making rough draft of hotbar.
Have since moved to inheritence model
*/

public interface IThrowable
{
    void Throw(bool direction);
    void Drop();
}
