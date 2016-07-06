using UnityEngine;
using System.Collections;

public class AttackTrigger : MonoBehaviour {
    //ловит момент прикосновения триггера с коллайдером другого игрока
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))//Проверка что тэг соответствует игроку
        {
            //Если имена кланов совпадают, происходит выход из метода
            if (gameObject.GetComponentInParent<PlayerUpdate>().clanName.Equals(col.GetComponent<PlayerUpdate>().clanName) || col.GetComponent<PlayerUpdate>().GetDeath())
            {
                return;
            }

            bool block = col.GetComponent<Animator>().GetBool("Protect");//получение ответа стоил ли защита у противника
            float s = col.transform.localScale.x;//получение куда смотрит противник 
			int n = 30;//отброс при атаке
			if (!block)
				n = 300;//если блок не стоит, отброс увеличивается
            //стоит ли пользователь перед лицом врага
            if ((gameObject.transform.position.x > col.transform.position.x && s > 0 && block) || (gameObject.transform.position.x < col.transform.position.x && s < 0 && block) || !block) 
			{
				if (gameObject.transform.position.x > col.transform.position.x) 
				{
					col.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-n, 0));
					if (block)
						return;
				} 
				else 
				{
					col.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (n, 0));
					if (block)
						return;
				}

            }
            object[] tempStorage = new object[3];
            tempStorage[0] = col.transform.name;//имя кому нанесен урон
            tempStorage[1] = gameObject.GetComponentInParent<PlayerUpdate>().transform.name;//имя пользователя
            tempStorage[2] = gameObject.GetComponentInParent<PlayerUpdate>().player.damage;//урон
            gameObject.SendMessageUpwards("Hit", tempStorage);//отправка сообщения кому нанесен урон
        }
    }
}