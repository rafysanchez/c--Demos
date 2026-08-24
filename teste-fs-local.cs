using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		ApplicationConfiguration.Initialize();

		using var selecionarArquivo = new OpenFileDialog
		{
			Title = "Selecione um arquivo de texto",
			Filter = "Arquivos de texto (*.txt)|*.txt|Todos os arquivos (*.*)|*.*",
			CheckFileExists = true,
			Multiselect = false
		};

		if (selecionarArquivo.ShowDialog() != DialogResult.OK)
			return;

		using var selecionarPasta = new FolderBrowserDialog
		{
			Description = "Escolha a pasta onde os arquivos de saida serao criados",
			ShowNewFolderButton = true
		};

		if (selecionarPasta.ShowDialog() != DialogResult.OK)
			return;

		try
		{
			string caminhoOrigem = selecionarArquivo.FileName;
			string pastaDestino = selecionarPasta.SelectedPath;

			FileInfo arquivoOrigem = new(caminhoOrigem);
			string conteudo = File.ReadAllText(caminhoOrigem, Encoding.UTF8);
			string nomeBase = Path.GetFileNameWithoutExtension(caminhoOrigem);
			string extensao = Path.GetExtension(caminhoOrigem);
			string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

			string pastaBackup = Path.Combine(pastaDestino, "backup");
			string pastaProcessados = Path.Combine(pastaDestino, "processados");

			Directory.CreateDirectory(pastaBackup);
			Directory.CreateDirectory(pastaProcessados);

			string caminhoBackup = Path.Combine(pastaBackup, $"{nomeBase}_{timestamp}{extensao}");
			string caminhoCopiaEditada = Path.Combine(pastaProcessados, $"{nomeBase}_editado{extensao}");
			string caminhoRelatorio = Path.Combine(pastaDestino, "relatorio.txt");

			File.Copy(caminhoOrigem, caminhoBackup, overwrite: true);

			string cabecalho = $"Arquivo original: {arquivoOrigem.Name}{Environment.NewLine}";
			string rodape =
				$"{Environment.NewLine}{Environment.NewLine}-----{Environment.NewLine}" +
				$"Copia gerada em {DateTime.Now:dd/MM/yyyy HH:mm:ss}{Environment.NewLine}" +
				$"Tamanho original: {arquivoOrigem.Length} bytes{Environment.NewLine}";

			File.WriteAllText(caminhoCopiaEditada, cabecalho + conteudo + rodape, Encoding.UTF8);

			string linhaRelatorio =
				$"[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] " +
				$"Origem={caminhoOrigem} | Backup={caminhoBackup} | Copia={caminhoCopiaEditada}";
			File.AppendAllText(caminhoRelatorio, linhaRelatorio + Environment.NewLine, Encoding.UTF8);

			string[] linhasGeradas = File.ReadAllLines(caminhoCopiaEditada, Encoding.UTF8);
			string primeiraLinha = linhasGeradas.Length > 0 ? linhasGeradas[0] : "(arquivo vazio)";

			MessageBox.Show(
				$"Operacoes concluidas com sucesso.{Environment.NewLine}{Environment.NewLine}" +
				$"Arquivo original: {arquivoOrigem.FullName}{Environment.NewLine}" +
				$"Ultima modificacao: {arquivoOrigem.LastWriteTime:dd/MM/yyyy HH:mm:ss}{Environment.NewLine}" +
				$"Backup criado em: {caminhoBackup}{Environment.NewLine}" +
				$"Copia editada em: {caminhoCopiaEditada}{Environment.NewLine}" +
				$"Relatorio em: {caminhoRelatorio}{Environment.NewLine}" +
				$"Primeira linha da copia: {primeiraLinha}",
				"Exemplo de File e Directory",
				MessageBoxButtons.OK,
				MessageBoxIcon.Information);
		}
		catch (UnauthorizedAccessException)
		{
			MessageBox.Show(
				"Sem permissao para acessar o arquivo ou a pasta selecionada.",
				"Erro",
				MessageBoxButtons.OK,
				MessageBoxIcon.Error);
		}
		catch (DirectoryNotFoundException erro)
		{
			MessageBox.Show(
				$"Pasta nao encontrada:{Environment.NewLine}{erro.Message}",
				"Erro",
				MessageBoxButtons.OK,
				MessageBoxIcon.Error);
		}
		catch (IOException erro)
		{
			MessageBox.Show(
				$"Erro de leitura ou gravacao:{Environment.NewLine}{erro.Message}",
				"Erro",
				MessageBoxButtons.OK,
				MessageBoxIcon.Error);
		}
		catch (Exception erro)
		{
			MessageBox.Show(
				$"Erro inesperado:{Environment.NewLine}{erro.Message}",
				"Erro",
				MessageBoxButtons.OK,
				MessageBoxIcon.Error);
		}
	}
}
