namespace LeMinhHuy.AI
{
	//Does core damage to the main health
	//Unit dies once all health is lost
	public interface ICoreDamageable
	{
		void TakeCoreDamage(float damage);
	}
}
