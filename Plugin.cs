using BepInEx;
using System.Security.Permissions;
using UnityEngine;
using RWCustom;
using System;

#pragma warning disable CS0618 // Do not remove the following line.
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace SpriteReplacementCo;

[BepInPlugin("cbartificer.fix", "Artificer Scar Fix", "1.0.1")]
public class ArtificerScarFix : BaseUnityPlugin
{
    public void OnEnable()
    {
        On.PlayerGraphics.DrawSprites += (orig, self, sLeaser, rCam, timeStacker, camPos) => {
            orig(self, sLeaser, rCam, timeStacker, camPos);
            if(self.player.slugcatStats.name.ToString() == "Artificer" && sLeaser.sprites.Length > 12) {
                Vector2 vector = sLeaser.sprites[1].GetPosition() - (sLeaser.sprites[9].GetPosition());
                Vector2 vectorNormalized = vector.normalized;
                Vector2 perpVector = Custom.PerpendicularVector(vectorNormalized);
                //base.Logger.LogDebug(sLeaser.sprites[9].rotation <= 0 && sLeaser.sprites[9].rotation >= -90f);
                //base.Logger.LogDebug((Mathf.Abs(sLeaser.sprites[3].rotation)-90)/90f);
                //base.Logger.LogDebug(vectorNormalized);
                //base.Logger.LogDebug(perpVector);
                if (self.player?.room?.gravity != 0 && !self.player.submerged) {
                    if (sLeaser.sprites[3].rotation >= 0 && sLeaser.sprites[3].rotation <= 90f) {
                        sLeaser.sprites[12].SetPosition(sLeaser.sprites[9].GetPosition() + (vectorNormalized * Mathf.Lerp(-2,-5,sLeaser.sprites[3].rotation/90f)) + (perpVector * Mathf.Lerp(4,1,sLeaser.sprites[3].rotation/90f)));
                        sLeaser.sprites[12].scaleX = Mathf.Lerp(1f,0.4f,sLeaser.sprites[3].rotation/90f);
                    }
                    else if (sLeaser.sprites[3].rotation <= 0 && sLeaser.sprites[3].rotation >= -90f) {
                        sLeaser.sprites[12].SetPosition(sLeaser.sprites[9].GetPosition() + (vectorNormalized * Mathf.Lerp(-2,0,Mathf.Abs(sLeaser.sprites[3].rotation)/90f)) + (perpVector * Mathf.Lerp(4,0,Mathf.Abs(sLeaser.sprites[3].rotation)/90f)));
                        sLeaser.sprites[12].scaleX = 1;
                    }
                    else if (sLeaser.sprites[3].rotation <= -90f && sLeaser.sprites[3].rotation >= -180) {
                        sLeaser.sprites[12].SetPosition(sLeaser.sprites[9].GetPosition() + (vectorNormalized * Mathf.Lerp(0,-4,(Mathf.Abs(sLeaser.sprites[3].rotation)-90f)/90f)) + (perpVector * Mathf.Lerp(0,-3,(Mathf.Abs(sLeaser.sprites[3].rotation)-90f)/90f)));
                        sLeaser.sprites[12].scaleX = 1;
                    }
                    else if (sLeaser.sprites[3].rotation >= 90f && sLeaser.sprites[3].rotation <= 180) {
                        sLeaser.sprites[12].SetPosition(sLeaser.sprites[9].GetPosition() + (vectorNormalized * Mathf.Lerp(-5,-4,(Mathf.Abs(sLeaser.sprites[3].rotation)-90f)/90f)) + (perpVector * Mathf.Lerp(0,-3,(Mathf.Abs(sLeaser.sprites[3].rotation)-90f)/90f)));
                        sLeaser.sprites[12].scaleX = Mathf.Lerp(0.4f,1f,(Mathf.Abs(sLeaser.sprites[3].rotation)-90f)/90f);
                    }
                    //base.Logger.LogDebug(sLeaser.sprites[3].rotation);
                }
                
                else if (self.player?.room?.gravity == 0) {
                    if (sLeaser.sprites[3].rotation >= 0 && sLeaser.sprites[3].rotation <= 90f) {
                        sLeaser.sprites[12].SetPosition(sLeaser.sprites[9].GetPosition() + (vectorNormalized * Mathf.Lerp(-2,-4,sLeaser.sprites[3].rotation/90f)) + (perpVector * Mathf.Lerp(4,0.5f,sLeaser.sprites[3].rotation/90f)));
                    }
                    else if (sLeaser.sprites[3].rotation <= 0 && sLeaser.sprites[3].rotation >= -90f) {
                        sLeaser.sprites[12].SetPosition(sLeaser.sprites[9].GetPosition() + (vectorNormalized * Mathf.Lerp(-2,-1,Mathf.Abs(sLeaser.sprites[3].rotation)/90f)) + (perpVector * Mathf.Lerp(4,3,Mathf.Abs(sLeaser.sprites[3].rotation)/90f)));
                    }
                    else if (sLeaser.sprites[3].rotation <= -90f && sLeaser.sprites[3].rotation >= -180) {
                        sLeaser.sprites[12].SetPosition(sLeaser.sprites[9].GetPosition() + (vectorNormalized * Mathf.Lerp(-1,-3,(Mathf.Abs(sLeaser.sprites[3].rotation)-90f)/90f)) + (perpVector * Mathf.Lerp(3,2,(Mathf.Abs(sLeaser.sprites[3].rotation)-90f)/90f)));
                    }
                    else if (sLeaser.sprites[3].rotation >= 90f && sLeaser.sprites[3].rotation <= 180) {
                        sLeaser.sprites[12].SetPosition(sLeaser.sprites[9].GetPosition() + (vectorNormalized * Mathf.Lerp(-4,-3,(Mathf.Abs(sLeaser.sprites[3].rotation)-90f)/90f)) + (perpVector * Mathf.Lerp(1,3,(Mathf.Abs(sLeaser.sprites[3].rotation)-90f)/90f)));
                    }
                    sLeaser.sprites[12].scaleX = 1;
                }

                else if (self.player.submerged) {
                    sLeaser.sprites[12].SetPosition(sLeaser.sprites[9].GetPosition() + new Vector2(4f, sLeaser.sprites[9].GetPosition().y < sLeaser.sprites[1].GetPosition().y ? -2f : 2f));
                    sLeaser.sprites[12].scaleX = 1;
                }
            }
        };
    }
}