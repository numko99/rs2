import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:provider/provider.dart';
import 'package:ui/services/arrangment_provider.dart';
import 'package:ui/widgets/Layout/layout.dart';

class ArrangementAddEditPage extends StatefulWidget {
  const ArrangementAddEditPage({super.key});

  @override
  State<ArrangementAddEditPage> createState() => _ArrangementAddEditPageState();
}

class _ArrangementAddEditPageState extends State<ArrangementAddEditPage> {
  final _formKey = GlobalKey<FormBuilderState>();

  ArrangmentProvider? _arrangmentProvider;
  bool displayLoader = true;

  TextEditingController searchController = TextEditingController();

  @override
  void initState() {
    super.initState();
    _arrangmentProvider = context.read<ArrangmentProvider>();
    loadData();
  }

  @override
  Widget build(BuildContext context) {
    return Layout(
      body: AlertDialog(
        title: Column(
          children: [
            Text('Dodaj agenciju'),
            SizedBox(height: 10),
            Icon(
              Icons.card_travel,
              color: Colors.amber,
              size: 50,
            ),
          ],
        ),
        contentPadding: const EdgeInsets.fromLTRB(30.0, 5, 30.0, 40),
        content: Container(
          width: 600.0,
          child: FormBuilder(
            key: _formKey,
            child: Column(),
          ),
        ),
      ),
    );
  }

  Future<void> loadData() async {
    try {
      setState(() {
        displayLoader = true;
      });
    } catch (error) {
      print('Error loading data: $error');

      ScaffoldMessenger.of(context).showSnackBar(const SnackBar(
        content: Center(child: Text("Došlo je do greške na serveru")),
        backgroundColor: Colors.red,
      ));
    } finally {
      setState(() {
        displayLoader = false;
      });
    }
  }
}
