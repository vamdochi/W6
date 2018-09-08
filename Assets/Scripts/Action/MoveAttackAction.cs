using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAttackAction : MoveAction {

    private Vector2 moveTarget = Vector2.negativeInfinity;
    private bool isDoMoveAttack = false;

    protected override void LoadResource()
    {
        _animInfos.InitAnimMaxSize(2, NormalDir.MAX);

        _animInfos.RegisterAnimInfo(0, NormalDir.TOP, GetResourcePath()     + "Move/Top");
        _animInfos.RegisterAnimInfo(0, NormalDir.DOWN, GetResourcePath()    + "Move/Down");
        _animInfos.RegisterAnimInfo(0, NormalDir.BESIDE, GetResourcePath()  + "Move/Beside");

        _animInfos.RegisterAnimInfo(1, NormalDir.TOP, GetResourcePath()     + "Attack/Top");
        _animInfos.RegisterAnimInfo(1, NormalDir.DOWN, GetResourcePath()    + "Attack/Down");
        _animInfos.RegisterAnimInfo(1, NormalDir.BESIDE, GetResourcePath()  + "Attack/Beside");
    }

    public override bool Move(Vector3 moveDir)
    {
        if ( base.Move(moveDir) == false )
        {
            if (!IsCanDoAction())
                return false;

            int moveRow = (int)(_thisObject.Row + moveDir.x);
            int moveCol = (int)(_thisObject.Col + moveDir.y);

            BaseObject targetObject = TileManager.Get.GetObject(moveRow, moveCol);

            moveTarget = new Vector2(moveRow, moveCol);

            if ( targetObject is Player)
            {
                Player targetPlayer = targetObject as Player;

//                moveRow = (int)(targetPlayer.Row + moveDir.x);
//                moveCol = (int)(targetPlayer.Col + moveDir.y);
                LockObject();
                InternalOnlyMove(_thisObject.transform.position, TileManager.Get.GetTilePosition(moveRow, moveCol));
                isDoMoveAttack = true;

                return true;
     //           targetPlayer.KnockBackAction.Move( );
            }
        }
        else
        {
            isDoMoveAttack = false;
        }

        return false;
    }

    protected override void OnEndMove()
    {
        if (isDoMoveAttack)
        {
            if (TileManager.Get.IsCanMove((int)moveTarget.x, (int)moveTarget.y))
            {
                TileManager.Get.MoveObject(_thisObject, (int)moveTarget.x, (int)moveTarget.y);
            }
            else
            {
                isDoMoveAttack = false;
                // 일단 이것도 임시로
                InternalOnlyMove(_thisObject.transform.position, TileManager.Get.GetTilePosition(_thisObject.Row, _thisObject.Col));
            }
        }
        else
        {
            base.OnEndMove();
        }
    }

    protected override void CalcPosition(float fPercentage)
    {
        // 일단임시로씀
        base.CalcPosition(fPercentage);
    }
}
