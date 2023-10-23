using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        double preçoInicial, preçoPorHora;
        int opção;

        Console.WriteLine("Seja bem-vindo ao sistema de estacionamento!");

        while (true)
        {
            try
            {
                Console.Write("Digite o preço inicial: ");
                preçoInicial = Convert.ToDouble(Console.ReadLine());
                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("ERRO: Por favor, digite o preço inicial do estacionamento.");
                Thread.Sleep(1500);
                Console.Clear();
            }
        }

        while (true)
        {
            try
            {
                Console.Write("Digite o preço por hora: ");
                preçoPorHora = Convert.ToDouble(Console.ReadLine());
                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("ERRO: Por favor, digite o preço por hora do estacionamento.");
                Thread.Sleep(1500);
                Console.Clear();
            }
        }

        Estacionamento estacionamento = new Estacionamento(preçoInicial, preçoPorHora);

        while (true)
        {
            opção = Visual.Menu("Estacionamento", "Digite o número da opção desejada", "Cadastrar Veículos", "Listar Veículos", "Remover Veículos", "Encerrar");

            switch (opção)
            {
                case 1:
                    CadastrarVeiculo(estacionamento);
                    break;
                case 2:
                    ListarVeiculos(estacionamento);
                    break;
                case 3:
                    RemoverVeiculo(estacionamento);
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opção indisponível.");
                    break;
            }

            Console.WriteLine("Pressione Enter para continuar.");
            Console.ReadLine();
        }
    }

    static void CadastrarVeiculo(Estacionamento estacionamento)
    {
        Console.Clear();
        Console.Write("Digite a placa do veículo: ");
        string veiculoDigitado = Console.ReadLine();
        estacionamento.AdicionarVeiculo(veiculoDigitado);
        Console.WriteLine("Veículo cadastrado com sucesso.");
    }

    static void ListarVeiculos(Estacionamento estacionamento)
    {
        Console.Clear();
        var placas = estacionamento.ListaDePlacasDosVeiculosEstacionados();
        if (placas.Count == 0)
        {
            Console.WriteLine("Nenhum veículo estacionado.");
        }
        else
        {
            Visual.Titulo("Veículos Estacionados");
            for (int i = 0; i < placas.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {placas[i]}");
            }
        }
        Visual.Linha();
    }

    static void RemoverVeiculo(Estacionamento estacionamento)
    {
        Console.Clear();
        Console.Write("Digite a placa do veículo para removê-lo: ");
        string veiculoASerVerificadoERemovido = Console.ReadLine();

        byte horasQueOVeiculoFicouEstacionado;
        while (true)
        {
            try
            {
                Console.Clear();
                Console.Write("Digite a quantidade de horas que o veículo permaneceu no estacionamento: ");
                horasQueOVeiculoFicouEstacionado = Convert.ToByte(Console.ReadLine());
                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("ERRO: digite o número de horas que o veículo ficou estacionado.");
                Thread.Sleep(1500);
            }
            catch (OverflowException)
            {
                Console.WriteLine("ERRO: Digite um valor válido.");
                Thread.Sleep(1500);
            }
        }

        bool veiculoEstaNoEstacionamento = estacionamento.VerificaçãoEExclusãoDoVeiculo(veiculoASerVerificadoERemovido);

        if (veiculoEstaNoEstacionamento)
        {
            double preçoACobrar = estacionamento.RealizarCobrança(horasQueOVeiculoFicouEstacionado);
            Console.WriteLine($"Remoção do veículo realizada com sucesso! Preço a cobrar: {preçoACobrar:C}");
        }
        else
        {
            Console.WriteLine("Veículo não encontrado.");
        }
    }
}

class Estacionamento
{
    public List<string> ListaDasPlacasDosVeículos = new List<string>();
    public double PreçoBase;
    public double PreçoHoras;

    public Estacionamento(double preçoBase, double preçoHoras)
    {
        PreçoBase = preçoBase;
        PreçoHoras = preçoHoras;
    }

    public bool VerificaçãoEExclusãoDoVeiculo(string placaDoVeiculo)
    {
        if (ListaDasPlacasDosVeículos.Contains(placaDoVeiculo))
        {
            ListaDasPlacasDosVeículos.Remove(placaDoVeiculo);
            return true;
        }
        return false;
    }

    public double RealizarCobrança(short horasNoEstacionamento)
    {
        double preçoTotal = PreçoHoras * horasNoEstacionamento + PreçoBase;
        return preçoTotal;
    }

    public void AdicionarVeiculo(string placaDoVeiculo)
    {
        ListaDasPlacasDosVeículos.Add(placaDoVeiculo);
    }

    public List<string> ListaDePlacasDosVeiculosEstacionados()
    {
        return ListaDasPlacasDosVeículos;
    }
}

class Visual
{
    public static void Linha()
    {
        Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
    }

    public static void Titulo(string titulo)
    {
        Console.WriteLine($"=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=\n\t{titulo}\n=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
    }

    public static byte Menu(string titulo, string mensagemDeEscolha, params string[] opçõesDoMenu)
    {
        byte escolha;
        Titulo(titulo);
        for (int i = 0; i < opçõesDoMenu.Length; i++)
        {
            Console.WriteLine($"{i + 1} - {opçõesDoMenu[i]}");
        }
        Linha();
        Console.Write($"{mensagemDeEscolha}: ");
        escolha = Convert.ToByte(Console.ReadLine());
        return escolha;
    }
}
