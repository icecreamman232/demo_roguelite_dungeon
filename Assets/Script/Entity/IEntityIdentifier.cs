using System;
using UnityEngine;

/// <summary>
/// Generic interface for an entity that will contain certain methods
/// to define explicitly who this entity is
/// </summary>
public interface IEntityIdentifier
{
    bool IsPlayer();
}
