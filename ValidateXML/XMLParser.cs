using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ValidateXML
{
    class XMLParser
    {
        string xmlPath;

        string path = "//tp:Ficha/tp:Dados_da_ficha/";
        string data = "tp:data/tp:Any_event_as_Point_Event/tp:data/";
        string value = "tp:value/";

        string genero;
        string raca;
        string idade;

        string alteracaoNaImagem;
        string exame;

        string rac;

        string tfg;
        string categoriaTfg;

        string creatinina;

        string albumina;
        string categoriaAlbumina;

        string equ;
        string caracteristicasEqu;

        string diabete;
        string litiase;
        string infeccao;
        string hipertensao;
        string policistica;

        public XMLParser(string xmlPath)
        {
            this.xmlPath = xmlPath;
        }

        public void Run()
        {
            getDatas();
            parseToOWL();
        }

        void getDatas()
        {
            XmlReader reader = XmlReader.Create(xmlPath);
            XDocument doc = XDocument.Load(reader);
            XmlNameTable nameTable = reader.NameTable;
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(nameTable);
            namespaceManager.AddNamespace("tp", "http://schemas.oceanehr.com/templates");
            namespaceManager.AddNamespace("v1", "http://schemas.openehr.org/v1");

            //Dados do paciente
            try
            {
                this.genero = doc.XPathSelectElement(path + "tp:Paciente/tp:Dados_do_paciente/" + data + "tp:Gênero/" + value + "tp:defining_code/tp:code_string", namespaceManager).Value;
                this.raca = doc.XPathSelectElement(path + "tp:Paciente/tp:Dados_do_paciente/" + data + "tp:Raça/" + value + "tp:defining_code/tp:code_string", namespaceManager).Value;
                this.idade = doc.XPathSelectElement(path + "tp:Paciente/tp:Dados_do_paciente/" + data + "tp:Idade/" + value + "tp:magnitude", namespaceManager).Value;
            }
            catch (NullReferenceException e)
            {
                this.genero = this.raca = this.idade = null;
            }

            //Alteraçao de imagem
            try
            { 
                this.alteracaoNaImagem = doc.XPathSelectElement(path + "tp:Alteracao_de_imagem/tp:Alteracao_da_imagem/" + data + "tp:Alteração_na_imagem/" + value + "v1:value", namespaceManager).Value;
            }
            catch (NullReferenceException e) {
                this.alteracaoNaImagem = null;
            }

            if (alteracaoNaImagem != null) {
                try
                {
                    this.exame = doc.XPathSelectElement(path + "tp:Alteracao_de_imagem/tp:Alteracao_da_imagem/" + data + "tp:Exame/" + value + "v1:value", namespaceManager).Value;

                }
                catch (NullReferenceException e) {
                    this.exame = "";
                }
            }

            //Exames
            //RAC
            try
            {
                this.rac = doc.XPathSelectElement(path + "tp:Exames/tp:RAC/" + data + "tp:Relação_albuminuria_creatinuria/" + value + "tp:magnitude", namespaceManager).Value;
            }
            catch (NullReferenceException e) {
                this.rac = null;
            }
            //TFG
            try
            {
                this.tfg = doc.XPathSelectElement(path + "tp:Exames/tp:TFG/" + data + "tp:TFG/" + value + "tp:magnitude", namespaceManager).Value;
                this.categoriaTfg = doc.XPathSelectElement(path + "tp:Exames/tp:TFG/" + data + "tp:Categoria_TFG/" + value + "tp:defining_code/tp:code_string", namespaceManager).Value;
            }
            catch (NullReferenceException e) {
                this.tfg = null;
            }

            if(tfg != null) this.categoriaTfg = doc.XPathSelectElement(path + "tp:Exames/tp:TFG/" + data + "tp:Categoria_TFG/" + value + "tp:defining_code/tp:code_string", namespaceManager).Value;

            //Creatinina
            try
            {
                this.creatinina = doc.XPathSelectElement(path + "tp:Exames/tp:Cretinina_Serica/" + data + "tp:Creatinina/" + value + "tp:magnitude", namespaceManager).Value;
            }
            catch (NullReferenceException e)
            {

            }

            //Nivel de albuminuria
            try
            {
                this.albumina = doc.XPathSelectElement(path + "tp:Exames/tp:Nível_de_albuminuria/" + data + "tp:Nível_de_albumina/" + value + "tp:magnitude", namespaceManager).Value;
                this.categoriaAlbumina = doc.XPathSelectElement(path + "tp:Exames/tp:Nível_de_albuminuria/" + data + "tp:Categoria/" + value + "tp:defining_code/tp:code_string", namespaceManager).Value;
            }
            catch (NullReferenceException e)
            {

            }
            //EQU EAS
            try
            {
                this.equ = doc.XPathSelectElement(path + "tp:Exames/tp:EQU_EAS_Urinatipo1/" + data + "tp:Elementos_Anormais_do_Sedimento/" + value + "v1:value", namespaceManager).Value;
            }
            catch (NullReferenceException e) {

            }

            if(equ != null && equ == "true") this.caracteristicasEqu = doc.XPathSelectElement(path + "tp:Exames/tp:EQU_EAS_Urinatipo1/" + data + "tp:Características/" + value + "v1:value", namespaceManager).Value;

            //Fatores de Risco
            try
            {
                this.diabete = doc.XPathSelectElement(path + "tp:Fatores_de_risco/tp:Outros_problemas_do_paciente/" + data + "tp:Diabetes_Mellitus/" + value + "v1:value", namespaceManager).Value;
                this.policistica = doc.XPathSelectElement(path + "tp:Fatores_de_risco/tp:Outros_problemas_do_paciente/" + data + "tp:Doença_policistica_renal/" + value + "v1:value", namespaceManager).Value;
                this.litiase = doc.XPathSelectElement(path + "tp:Fatores_de_risco/tp:Outros_problemas_do_paciente/" + data + "tp:Litiase_Renal/" + value + "v1:value", namespaceManager).Value;
                this.infeccao = doc.XPathSelectElement(path + "tp:Fatores_de_risco/tp:Outros_problemas_do_paciente/" + data + "tp:Infecção_urinária_recorrente/" + value + "v1:value", namespaceManager).Value;
                this.hipertensao = doc.XPathSelectElement(path + "tp:Fatores_de_risco/tp:Outros_problemas_do_paciente/" + data + "tp:Hipertensão_arterial_sistemática/" + value + "v1:value", namespaceManager).Value;
            } catch (NullReferenceException e)
            {
                this.diabete = this.policistica = this.litiase = this.infeccao = this.hipertensao = null;
            }

            Print();
        }

        void parseToOWL()
        {
            XmlReader reader = XmlReader.Create("arquivos/Ontodrc1.1.owl");
            XDocument doc = XDocument.Load(reader);
            XmlNameTable nameTable = reader.NameTable;
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(nameTable);
            namespaceManager.AddNamespace("owl", "http://www.w3.org/2002/07/owl#");

            XNamespace owl = "http://www.w3.org/2002/07/owl#";

            XElement element;

            element = doc.XPathSelectElement("//owl:Ontology", namespaceManager);

            string generoURI = "http://purl.obolibrary.org/obo/ontoneo.owl#";
            string racaURI = "http://purl.obolibrary.org/obo/ontoneo.owl#";


            //Dados do paciente
            //Genero
            if (genero != null)
            {
                if (genero == "at0007") generoURI += "Masculino";
                else generoURI += "Feminino";
                element.Add(new XElement(owl + "Declaration",
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#genero"))));
                element.Add(new XElement(owl + "ClassAssertion",
                    new XElement(owl + "Class", new XAttribute("URI", "#Genero")),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#genero"))));
                element.Add(new XElement(owl + "ObjectPropertyAssertion",
                    new XElement(owl + "ObjectProperty", new XAttribute("URI", "http://purl.obolibrary.org/obo/ontoneo.owl#temGenero")),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#genero")),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", generoURI))));
            }

            //Raca
            if (raca != null)
            {
                if (raca == "at0009") racaURI += "Preta";
                else if (raca == "at0012") racaURI += "Parda";
                else if (raca == "at0010") racaURI += "Branca";
                else if (raca == "at0011") racaURI += "Amarela";
                element.Add(new XElement(owl + "Declaration",
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#raca"))));
                element.Add(new XElement(owl + "ClassAssertion",
                    new XElement(owl + "Class", new XAttribute("URI", "#Raca")),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#raca"))));
                element.Add(new XElement(owl + "ObjectPropertyAssertion",
                    new XElement(owl + "ObjectProperty", new XAttribute("URI", "http://purl.obolibrary.org/obo/ontoneo.owl#temRaca")),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#raca")),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", racaURI))));
            }

            //Idade
            if (idade != null)
            {
                element.Add(new XElement(owl + "Declaration",
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#idade"))));
                element.Add(new XElement(owl + "ClassAssertion",
                    new XElement(owl + "Class", new XAttribute("URI", "#Idade")),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#idade"))));
                element.Add(new XElement(owl + "DataPropertyAssertion",
                    new XElement(owl + "DataProperty", new XAttribute("abbreviatedIRI", "owl:topDataProperty")),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#idade")),
                    new XElement(owl + "Literal", new XAttribute("datatypeIRI", "http://www.w3.org/2001/XMLSchema#int"), idade)));
            }

            //Alteracao de imagem
            if(alteracaoNaImagem != null && alteracaoNaImagem == "true")
            { 
                element.Add(new XElement(owl + "Declaration",
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#alteracao_de_imagem"))));
                element.Add(new XElement(owl + "ClassAssertion",
                    new XElement(owl + "Class", new XAttribute("URI", "#AlteracaoDaImagem")),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#alteracao_de_imagem"))));
            }

            //Exame
            //RAC
            if (rac != null)
            {
                element.Add(new XElement(owl + "Declaration",
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#rac"))));
                element.Add(new XElement(owl + "ClassAssertion",
                    new XElement(owl + "Class", new XAttribute("URI", "#RelacaoAlbuminaCreatinina")),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#rac"))));
            }

            //TFG
            if (tfg != null) {
                
                
                element.Add(new XElement(owl + "Declaration",
                new XElement(owl + "NamedIndividual", new XAttribute("URI", "#tfg"))));
                element.Add(new XElement(owl + "ClassAssertion",
                    new XElement(owl + "Class", new XAttribute("URI", "#" + getTFGClass())),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#tfg"))));
                element.Add(new XElement(owl + "SameIndividual",
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#" + getTFGIndividual())),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#tfg"))));
            }

            //Creatinina Serica
            if (creatinina != null)
            {
                element.Add(new XElement(owl + "Declaration",
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#creatinina"))));
                element.Add(new XElement(owl + "ClassAssertion",
                    new XElement(owl + "Class", new XAttribute("URI", "#CreatininaSerica")),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#creatinina"))));
                element.Add(new XElement(owl + "DataPropertyAssertion",
                    new XElement(owl + "DataProperty", new XAttribute("abbreviatedIRI", "owl:topDataProperty")),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#creatinina")),
                    new XElement(owl + "Literal", new XAttribute("datatypeIRI", "http://www.w3.org/2001/XMLSchema#int"), creatinina)));
            }

            if (categoriaAlbumina != null)
            {
                string classA = "#";
                if (categoriaAlbumina == "at0006") classA += "A1";
                else if (categoriaAlbumina == "at0007") classA += "A2";
                else if (categoriaAlbumina == "at0008") classA += "A3";

                element.Add(new XElement(owl + "Declaration",
                        new XElement(owl + "NamedIndividual", new XAttribute("URI", "#albumina"))));
                element.Add(new XElement(owl + "ClassAssertion",
                    new XElement(owl + "Class", new XAttribute("URI", classA)),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#albumina"))));
            }

            if (equ != null && equ == "true")
            {
                element.Add(new XElement(owl + "Declaration",
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#equ_eas"))));
                element.Add(new XElement(owl + "ClassAssertion",
                    new XElement(owl + "Class", new XAttribute("URI", "#EQU_EAS_UrinaTipo1")),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#equ_eas"))));
            }

            if(diabete != null && diabete == "true")
            {
                element.Add(new XElement(owl + "Declaration",
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#diabete"))));
                element.Add(new XElement(owl + "ClassAssertion",
                    new XElement(owl + "Class", new XAttribute("URI", "#DiabetesMellitus")),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#diabete"))));
            }

            if (litiase != null && litiase == "true")
            {
                element.Add(new XElement(owl + "Declaration",
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#litiase"))));
                element.Add(new XElement(owl + "ClassAssertion",
                    new XElement(owl + "Class", new XAttribute("URI", "#LitiaseRenal")),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#litiase"))));
            }

            if (policistica != null && policistica == "true")
            {
                element.Add(new XElement(owl + "Declaration",
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#policistica"))));
                element.Add(new XElement(owl + "ClassAssertion",
                    new XElement(owl + "Class", new XAttribute("URI", "#DoencaPolicisticaRenal")),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#policistica"))));
            }

            if (infeccao != null && infeccao == "true")
            {
                element.Add(new XElement(owl + "Declaration",
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#infeccao"))));
                element.Add(new XElement("ClassAssertion",
                    new XElement(owl + "Class", new XAttribute("URI", "#InfeccaoUrinariaRecorrente")),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#infeccao"))));
            }

            if (hipertensao != null && hipertensao == "true")
            {
                element.Add(new XElement(owl + "Declaration",
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#hipertensao"))));
                element.Add(new XElement("ClassAssertion",
                    new XElement(owl + "Class", new XAttribute("URI", "#HipertensaoArterialSistematica")),
                    new XElement(owl + "NamedIndividual", new XAttribute("URI", "#hipertensao"))));
            }

            doc.Save("arquivos/Resultado.owl");


        }

        string getTFGClass() {

            string tfgClass = "";
            
            switch (categoriaTfg)
            {
                case "at0013":
                    tfgClass = "TFG1";
                    break;
                case "at0015":
                    tfgClass = "TFG2";
                    break;
                case "at0025":
                    tfgClass = "TFG3A";
                    break;
                case "at0026":
                    tfgClass = "TFG3B";
                    break;
                case "at0027":
                    tfgClass = "TFG4";
                    break;
                case "at0028":
                    tfgClass = "TFG5";
                    break;
            }

            return tfgClass;
        }

        string getTFGIndividual()
        {

            string tfgIndvidual = "";
            switch (categoriaTfg)
            {
                case "at0013":
                    tfgIndvidual = "TFGI1";
                    break;
                case "at0015":
                    tfgIndvidual = "TFGI2";
                    break;
                case "at0025":
                    tfgIndvidual = "TFGI3A";
                    break;
                case "at0026":
                    tfgIndvidual = "TFGI3B";
                    break;
                case "at0027":
                    tfgIndvidual = "TFGI4";
                    break;
                case "at0028":
                    tfgIndvidual = "TFGI5";
                    break;


            }

            return tfgIndvidual;
        }

        public void Print()
        {
            Console.WriteLine("Dados do paciente: ");
            Console.WriteLine("Gênero: " + genero);
            Console.WriteLine("Raça: " + raca);
            Console.WriteLine("Idade: " + idade);

            Console.WriteLine();
            Console.WriteLine("Alteracao da imagem: ");
            Console.WriteLine("Teve alteração: " + alteracaoNaImagem);
            Console.WriteLine("Exame: " + exame);

            Console.WriteLine();
            Console.WriteLine("TFG: ");
            Console.WriteLine("Valor: " + tfg);
            Console.WriteLine("Categoria: " + categoriaTfg);

            Console.WriteLine();
            Console.WriteLine("RAC: ");
            Console.WriteLine("Valor: " + rac);

            Console.WriteLine();
            Console.WriteLine("Creatinina serica: ");
            Console.WriteLine("Valor: " + creatinina);

            Console.WriteLine();
            Console.WriteLine("Nivel de albuminuria: ");
            Console.WriteLine("Valor: " + albumina);
            Console.WriteLine("Categoria: " + categoriaAlbumina);

            Console.WriteLine();
            Console.WriteLine("EQU EAS: ");
            Console.WriteLine("Valor: " + equ);
            Console.WriteLine("Caracteristicas: " + caracteristicasEqu);

            Console.WriteLine();
            Console.WriteLine("Fatores de risco: ");
            Console.WriteLine("Diabetes: " + diabete);
            Console.WriteLine("Policistica: " + policistica);
            Console.WriteLine("Litiase: " + litiase);
            Console.WriteLine("Infecção: " + infeccao);

        }
    }
}
