import 'package:flutter/material.dart';
import 'package:flutter_multi_formatter/formatters/masked_input_formatter.dart';
import 'package:intl/intl.dart';
import 'package:iter_mobile/helpers/scaffold_messenger_helper.dart';
import 'package:iter_mobile/models/user.dart';
import 'package:iter_mobile/providers/user_provider.dart';
import 'package:provider/provider.dart'; // Dodajte ovo za formatiranje datuma

class EditProfilePage extends StatefulWidget {
  const EditProfilePage({super.key});

  @override
  _EditProfilePageState createState() => _EditProfilePageState();
}

class _EditProfilePageState extends State<EditProfilePage> {
  final _formKey = GlobalKey<FormState>();

  bool displayLoader = true;
  User? user;
  UserProvider? _userProvider;

  TextEditingController _nameController = TextEditingController();
  TextEditingController _surnameController = TextEditingController();
  TextEditingController _residencePlaceController = TextEditingController();
  TextEditingController _emailController = TextEditingController();
  TextEditingController _phoneController = TextEditingController();
  TextEditingController _birthDateController = TextEditingController();

  @override
  void initState() {
    super.initState();
    _userProvider = context.read<UserProvider>();
    loadData();
  }

  void loadData() async {
    try {
      var userTemp = await _userProvider?.getCurrentUser();

      if (userTemp != null) {
        setState(() {
          displayLoader = false;
          user = userTemp;

          _nameController.text = user!.firstName ?? '';
          _surnameController.text = user!.lastName ?? '';
          _emailController.text = user!.email ?? '';
          _phoneController.text = user!.phoneNumber ?? '';
          _residencePlaceController.text = user!.residencePlace;
          _birthDateController.text =
              DateFormat('dd.MM.yyyy').format(user!.birthDate);
        });
      }
    } catch (e) {}
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: Text('Uredi profil', style: TextStyle(color: Colors.white)),
        ),
        body: displayLoader
            ? const Center(child: CircularProgressIndicator())
            : Padding(
                padding: const EdgeInsets.all(16.0),
                child: Form(
                  key: _formKey,
                  child: ListView(
                    children: <Widget>[
                      TextFormField(
                        controller: _nameController,
                        decoration: InputDecoration(
                            labelText: 'Ime',
                            prefixIcon:
                                const Icon(Icons.person, color: Colors.amber),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(5.0),
                            )),
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Polje je obavezno';
                          }
                          return null;
                        },
                      ),
                      const SizedBox(height: 20),
                      TextFormField(
                        controller: _surnameController,
                        decoration: InputDecoration(
                            labelText: 'Prezime',
                            prefixIcon:
                                const Icon(Icons.person, color: Colors.amber),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(5.0),
                            )),
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Polje je obavezno';
                          }
                          return null;
                        },
                      ),
                      const SizedBox(height: 20),
                      TextFormField(
                        controller: _birthDateController,
                        decoration: InputDecoration(
                          labelText: 'Datum rođenja',
                          prefixIcon: const Icon(Icons.calendar_month,
                              color: Colors.amber),
                          border: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(5.0),
                          ),
                        ),
                        readOnly: true,
                        onTap: () async {
                          DateTime? pickedDate = await showDatePicker(
                            context: context,
                            initialDate: user!.birthDate,
                            firstDate: DateTime(1900),
                            lastDate: DateTime.now(),
                            builder: (BuildContext context, Widget? child) {
                              return Theme(
                                data: ThemeData
                                    .light(),
                                child: child!,
                              );
                            },
                          );

                          if (pickedDate != null) {
                            String formattedDate =
                                DateFormat('dd.MM.yyyy').format(pickedDate);
                            _birthDateController.text = formattedDate;
                          }
                        },
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Polje je obavezno';
                          }
                          return null;
                        },
                        keyboardType: TextInputType.none,
                      ),
                      const SizedBox(height: 20),
                      TextFormField(
                        controller: _residencePlaceController,
                        decoration: InputDecoration(
                            labelText: 'Mjesto prebivališta',
                            prefixIcon: const Icon(Icons.location_city,
                                color: Colors.amber),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(5.0),
                            )),
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Polje je obavezno';
                          }
                          return null;
                        },
                      ),
                      const SizedBox(height: 20),
                      TextFormField(
                        controller: _emailController,
                        decoration: InputDecoration(
                            labelText: 'Email',
                            prefixIcon:
                                const Icon(Icons.email, color: Colors.amber),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(5.0),
                            )),
                        keyboardType: TextInputType.emailAddress,
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Polje je obavezno';
                          } else {
                            String pattern =
                                r'\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b';
                            RegExp regex = RegExp(pattern);
                            if (!regex.hasMatch(value)) {
                              return 'Unesite validnu email adresu';
                            }
                          }
                          return null;
                        },
                      ),
                      const SizedBox(height: 20),
                      TextFormField(
                        controller: _phoneController,
                        inputFormatters: [
                          MaskedInputFormatter('000-000-0000'),
                        ],
                        decoration: InputDecoration(
                            labelText: 'Broj telefona',
                            prefixIcon: const Icon(
                              Icons.phone,
                              color: Colors.amber,
                            ),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(5.0),
                            )),
                        keyboardType: TextInputType.phone,
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Polje je obavezno';
                          }
                          final regex = RegExp(r'^\d{3}-\d{3}-\d{3,4}$');
                          if (!regex.hasMatch(value)) {
                            return 'Neispravan format';
                          }
                          return null;
                        },
                      ),
                    ],
                  ),
                ),
              ),
        bottomNavigationBar: BottomAppBar(
          child: Padding(
            padding: const EdgeInsets.all(16.0),
            child: ElevatedButton(
              onPressed: () => submit(),
              child: const Text('Sačuvaj promjene',
                  style: TextStyle(color: Colors.white)),
            ),
          ),
        ));
  }

  submit() async {
    if (_formKey.currentState?.validate() == false) {
      return;
    }

    Map<String, dynamic> request = {
      'firstName': _nameController.text,
      'lastName': _surnameController.text,
      'residencePlace': _residencePlaceController.text,
      'email': _emailController.text,
      'phoneNumber': _phoneController.text,
      'birthDate': _birthDateController.text,
    };

    try {
      await _userProvider?.updateCurrentUser(request);
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Uspješno ste uredili podatke",
          backgroundColor: Colors.green);
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške: ${error.toString()}",
          backgroundColor: Colors.red);
    } finally {
      setState(() {
        displayLoader = false;
      });
      if (mounted) {
        Navigator.of(context).pop(true);
      }
    }
  }
}
