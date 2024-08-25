import 'package:flutter/material.dart';
import 'package:number_paginator/number_paginator.dart';
import 'package:provider/provider.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/modals/custom_confirmation_modal.dart';
import 'package:ui/modals/insert_city_modal.dart';
import 'package:ui/modals/insert_country_modal.dart';
import 'package:ui/models/country.dart';
import 'package:ui/services/country_provider.dart';
import 'package:ui/widgets/search_button.dart';

class CountryDataTable extends StatefulWidget {
  const CountryDataTable({super.key});

  @override
  State<CountryDataTable> createState() => CountryDataTableState();
}

class CountryDataTableState extends State<CountryDataTable> {
  TextEditingController searchController = TextEditingController();

  CountryProvider? _countryProvider;
  List<Country> countries= [];
  int? countriesCount;

  bool _displayLoader = true;

  int _currentPage = 1;
  int _pageSize = 10;

  @override
  void initState() {
    super.initState();
    _countryProvider = context.read<CountryProvider>();
    initialLoad();
  }

  Future<void> initialLoad() async {
    setState(() {
      _displayLoader = true;
    });
    try {
      await search();

    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške",
          backgroundColor: Colors.red);
    } finally {
      setState(() {
        _displayLoader = false;
      });
    }
  }

  Future<void> search() async {
    var searchCities = await _countryProvider?.get({
      "currentPage": _currentPage,
      "pageSize": _pageSize,
      "name": searchController.text,
    });

    if (searchCities != null) {
      setState(() {
        countries = searchCities.result;
        countriesCount = searchCities.count;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          crossAxisAlignment: CrossAxisAlignment.end,
          children: <Widget>[
            Expanded(
              flex: 1,
              child: TextFormField(
                controller: searchController,
                decoration: const InputDecoration(
                  labelText: 'Unesite pretragu',
                ),
                onFieldSubmitted: (value) async {
                  await search();
                },
              ),
            ),
            const SizedBox(width: 20),
            SearchButton(
              onSearch: search,
            ),
          ],
        ),
        SizedBox(height: 20),
        Row(
          mainAxisAlignment: MainAxisAlignment.end,
          children: [
          ElevatedButton(
                      onPressed: () {
                        showDialog(
                          context: context,
                          builder: (BuildContext context) {
                            return InsertCountryModal(
                              onCompleted: () => search()
                            );
                          },
                        );
                      },
                      child: const Text(
                        "Dodaj državu",
                        style: TextStyle(color: Colors.white),
                      ),
                    ),
        ]),
        SizedBox(height: 20),
        _displayLoader
            ? const Center(
                child: Column(children: [CircularProgressIndicator()]))
            : Row(children: <Widget>[
                Expanded(
                  child: DataTable(
                    columns: const [
                      DataColumn(
                          label: Text('Naziv',
                              style: TextStyle(fontWeight: FontWeight.bold))),
                      DataColumn(label: Text('')),
                    ],
                    rows: countries
                        .map(
                          (country) => DataRow(
                            cells: [
                              DataCell(Text(country.name)),
                              DataCell(
                                SizedBox(
                                  width: double.infinity,
                                  child: Row(
                                    mainAxisAlignment: MainAxisAlignment.end,
                                    children: <Widget>[
                                      IconButton(
                                          icon: const Icon(Icons.edit_document),
                                          onPressed: () {
                                            showDialog(
                                              context: context,
                                              builder: (BuildContext context) {
                                                return InsertCountryModal(
                                                    onCompleted: () => search(),
                                                    id: country.id.toString());
                                              },
                                            );
                                          },
                                          tooltip: 'Uredi'),
                                      IconButton(
                                          icon: const Icon(Icons.delete),
                                          onPressed: () {
                                            showDialog(
                                              context: context,
                                              builder: (BuildContext context) {
                                                return CustomConfirmationDialog(
                                                  title: "Potvrda brisanja?",
                                                  content:
                                                      "Da li ste sigurni da želite obrisati državu ${country.name}?",
                                                  onConfirm: () async {
                                                    await delete(country.id);
                                                  },
                                                );
                                              },
                                            );
                                          },
                                          tooltip: 'Obriši'),
                                    ],
                                  ),
                                ),
                              ),
                            ],
                          ),
                        )
                        .toList(),
                  ),
                )
              ]),
        Row(children: <Widget>[
          Expanded(
            child: Align(
              alignment: Alignment.center,
              child: Container(
                constraints: const BoxConstraints(maxWidth: 400),
                child: Visibility(
                  visible:
                      countriesCount != null && countriesCount! > _pageSize,
                  child: NumberPaginator(
                    initialPage: 0,
                    numberPages:
                        (((countriesCount == 0 || countriesCount == null)
                                    ? 1
                                    : countriesCount!) /
                                _pageSize)
                            .ceil(),
                    onPageChange: (int index) {
                      setState(() {
                        _currentPage = index + 1;
                      });
                      search();
                    },
                  ),
                ),
              ),
            ),
          )
        ])
      ],
    );
  }

  Future<void> delete(id) async {
    try {
      setState(() {
        _displayLoader = true;
      });
      await _countryProvider?.delete(id);
      search();
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Uspješno ste obrisali državu",
          backgroundColor: Colors.green);
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške",
          backgroundColor: Colors.red);
    } finally {
      setState(() {
        _displayLoader = false;
      });
    }
  }

}

