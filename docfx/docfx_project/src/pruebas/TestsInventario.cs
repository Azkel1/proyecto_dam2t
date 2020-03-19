using NUnit.Framework;
using NUnit.Framework.Internal;
using ProyectoFinal_DI_AlexisSantana.model;

namespace ProyectoFinal_DI_AlexisSantana.pruebas
{
    [TestFixture]
    public class TestsInventario
    {
        Producto inv = new Producto();
        Producto inv2 = new Producto();

        //Comprobar si el ID es un valor numérico
        [Test]
        public void TestIDNumerico()
        {
            inv.Id = 3;
            Assert.IsTrue(inv.Id is int);
        }

        //Comprobar si el ID de un producto es superior al de otro
        [Test]
        public void TestIDSuperior()
        {
            inv.Id = 2;
            inv2.Id = 1;
            Assert.Greater(inv.Id, inv2.Id);
        }

        //Comprobar si dos productos tienen el mismo ID
        [Test]
        public void TestIDDuplicado()
        {
            inv.Id = 4;
            inv2.Id = 4;
            Assert.AreEqual(inv.Id, inv2.Id);
        }
    }
}
