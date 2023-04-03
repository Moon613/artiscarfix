using BepInEx;
using System.Security.Permissions;
using UnityEngine;
using RWCustom;
using System;

#pragma warning disable CS0618 // Do not remove the following line.
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace SpriteReplacementCo;

[BepInPlugin("cbartificer.fix", "SpriteReplacementCo", "1.0.0")]
public class SpriteReplacementCo : BaseUnityPlugin
{
    FAtlas catBoyAtlas;
    bool init = false;
    public SpriteReplacementCoOptions Options;
    public static SpriteReplacementCoOptions staticOptions;
    bool configWorking = false;
    bool flag = false;
    //string[] faceNames = new string[1];

    public void OnEnable()
    {
        On.RainWorld.OnModsInit += Init;
        On.OracleGraphics.InitiateSprites += (orig, self, sLeaser, rCam) => {
            orig(self, sLeaser, rCam);
            /*for(int i = 372; i < 374; i++) {
                base.Logger.LogDebug(i);
                base.Logger.LogDebug(sLeaser.sprites[i].element.name);
            }*/
        };
        On.OracleGraphics.DrawSprites += (orig, self, sLeaser, rCam, timeStacker, camPos) => {
            orig(self, sLeaser, rCam, timeStacker, camPos);
            /*for(int i = 372; i < 374; i++) {
                sLeaser.sprites[i].isVisible = false;
            }
            for(int i = 0; i < self.owner.bodyChunks.Length; i++) {
                base.Logger.LogDebug(i);
            }*/
            if(self.IsPebbles && !self.IsRottedPebbles && !self.IsSaintPebbles && !self.IsMoon && !self.IsPastMoon && staticOptions.isCatB.Value) {
                FSprite leftSprite = sLeaser.sprites[self.EyeSprite(0)];
                FSprite rightSprite = sLeaser.sprites[self.EyeSprite(1)];
                Vector2 vector = rightSprite.GetPosition() - leftSprite.GetPosition();
                Vector2 normalVector = vector.normalized;
                Vector2 perpVector = Custom.PerpendicularVector(normalVector);
                sLeaser.sprites[373].element = Futile.atlasManager.GetElementWithName("CatboyHead");;
                sLeaser.sprites[373].SetPosition(leftSprite.GetPosition() + (vector/2) + (perpVector*3));
                sLeaser.sprites[373].scaleX = 0.2f;
                sLeaser.sprites[373].scaleY = 0.2f;
                sLeaser.sprites[373].rotation = Mathf.Rad2Deg * -Mathf.Atan(vector.y / vector.x) - (rightSprite.GetPosition().x <= leftSprite.GetPosition().x? 180f:0f);
                sLeaser.sprites[373].color = Color.white;
            }
            //self.owner.room.AddObject(new Spark(rightSprite.GetPosition() + camPos, new Vector2(5,5), Color.blue, null, 10, 20));
            //self.owner.room.AddObject(new Spark(leftSprite.GetPosition() + camPos, new Vector2(-5,5), Color.cyan, null, 10, 20));
        };
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

            /*bool flag = false;
            for(int i = 0; i < faceNames.Length; i++) {
                base.Logger.LogDebug(faceNames[i]);
                if(faceNames[i] == sLeaser.sprites[9].element.name) {
                    flag = true;
                }
            }
            if(!flag) {
                Array.Resize<string>(ref faceNames, faceNames.Length+1);
                faceNames[faceNames.Length-1] = sLeaser.sprites[9].element.name;
            }*/
        };
        On.Player.Update += (orig, self, eu) => {
            orig(self, eu);
            //base.Logger.LogDebug(self.room.roomSettings.name);
            //self.room.PlaySound(Rickroll.rickroll, self.mainBodyChunk.pos, 0.1f, 1f);
            if (staticOptions.isCatB.Value && self.slugcatStats.name.ToString() != "Saint" && self.slugcatStats.name.ToString() != "Rivulet") {
                if (self?.room?.roomSettings.name == "SS_AI" && !flag) {
                    self.room.PlaySound(Rickroll.rickroll, self.mainBodyChunk.pos, 0.4f, 1.3f);
                    base.Logger.LogDebug("Tried to play music!");
                    flag = true;
                }
                else if (self?.room?.roomSettings.name != "SS_AI" && flag) {
                    flag = false;
                    base.Logger.LogDebug("Able to play music again");
                }
            }
        };
    }

    public void Init(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
        orig(self);
        
        if(!init) {
            init = true;
            try {
                Rickroll.RegisterValues();
                this.Options = new SpriteReplacementCoOptions(this, Logger);
                staticOptions = this.Options;
                MachineConnector.SetRegisteredOI("cbartificer.fix", Options);
                configWorking = true;
            }
            catch (Exception err) {
                base.Logger.LogError(err);
                configWorking = false;
            }
        }
        
        catBoyAtlas = Futile.atlasManager.LoadAtlas("assets/CatboyHead");

        if (catBoyAtlas == null) {
            Logger.LogWarning("CustomSpriteCo atlases not found! Reinstall the mod.");
        }
    }
}
public static class Rickroll
{
    public static void RegisterValues()
    {
        Rickroll.rickroll = new SoundID("rickroll", true);
    }
    public static SoundID rickroll;
}