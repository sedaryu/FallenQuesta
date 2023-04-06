using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCalculator
{
    public float ElapsedTime { get; private set; } = 0;
    public bool Player { get; private set; }
    public float FlyingTime { get; private set; }
    public float Speed { get; private set; }
    public float Rotation { get; private set; }
    public float Scale { get; private set; }

    public ProjectileCalculator(bool player, float flyingTime, float speed, float rotation, float scale)
    {
        Player = player;
        FlyingTime = flyingTime;
        Speed = speed;
        Rotation = rotation;
        Scale = scale;
    }

    public float[] Fly()
    {
        float[] move = new float[3];

        if (Player)
        {
            if (ElapsedTime < FlyingTime)
            {
                move[0] = Speed / FlyingTime;
                move[1] = Rotation / FlyingTime;
                move[2] = Scale / FlyingTime;
            }
            else if (FlyingTime <= ElapsedTime)
            {
                move[0] = Speed / FlyingTime;
                move[1] = Rotation / FlyingTime;
                move[2] = Scale / FlyingTime;
            }
        }
        else
        {
            if (ElapsedTime < FlyingTime)
            {
                move[0] = Speed / FlyingTime;
                move[1] = Rotation / FlyingTime;
                move[2] = Scale / FlyingTime;
            }
            else if (FlyingTime <= ElapsedTime && ElapsedTime < FlyingTime + 0.2f)
            {
                move[0] = -25f;
                move[1] = -500f;
                move[2] = 0;
            }
            else if (ElapsedTime >= FlyingTime + 0.2f)
            {
                move[0] = -25f;
                move[1] = 0f;
                move[2] = 0f;
            }
        }

        ElapsedTime += Time.deltaTime;

        return move;
    }

    public List<int> JudgeHit(float proj_x, List<float> targets_x, List<float> targets_scale)
    {
        List<int> hits = new List<int>();

        if (ElapsedTime >= FlyingTime)
        {
            for (int i = 0; i < targets_x.Count; i++)
            {
                if (targets_x[i] - targets_scale[i] <= proj_x && proj_x <= targets_x[i] + targets_scale[i])
                {
                    hits.Add(i);
                }
            }
        }

        return hits;
    }

    public bool JudgeHit(float proj_px, float proj_py, float proj_sx, float proj_sy, float target_px, float target_sx)
    {
        if (ElapsedTime >= FlyingTime)
        {
            if (proj_py - proj_sy <= -2.5f)
            {
                if (Mathf.Abs(proj_px - target_px) < proj_sx + target_sx)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
